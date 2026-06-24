using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows;

namespace MySimpleDisplayDriverChecker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // ファイルがドロップされた時に呼ばれるメソッド
        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                StatusText.Text = "処理中...";

                foreach (string filePath in files)
                {
                    ProcessFile(filePath);
                }

                StatusText.Text = "すべての処理が完了しました！";
            }
        }

        private void ProcessFile(string filePath)
        {
            try
            {
                // 1. 署名の検証

                // certの内容を保持するための変数
                string subjectName = "";

                {
                    // try構文内に書く

                    // 古い方法。インスタンス化してから、using構文で囲む方法
                    // using X509Certificate2 cert = new X509Certificate2(filePath);

                    // 新しい方法。スタティッククラスのLoadCertificateFromFileメソッドを呼び出して、using構文で囲む方法
                    // ここがハマったポイント。new()は不要。スタティック（静的）クラスであるため。
                    using X509Certificate2 cert = X509CertificateLoader.LoadCertificateFromFile(filePath);

                    bool isCertVaild = cert.Verify();

                    if (isCertVaild)
                    {
                        // 正しい場合
                        MessageBox.Show("このドライバーは信頼されています。");
                    }
                    else
                    {
                        // 署名はされているが、信頼できない場合
                        MessageBox.Show("エラー：証明書が不正または信頼されていません。");

                        // throwは想定外の異常に使うものなのでreturnを使う
                        //throw new Exception("終了します");
                        return;
                    }

                    // 検証用の鎖を用意する
                    X509Chain chain = new X509Chain();

                    // Buildメソッドに証明書を渡し、isChainVaildに代入
                    bool isChainValid = chain.Build(cert);

                    if (isChainValid)
                    {
                        // 正しい場合
                        MessageBox.Show("この証明書はルートまでつながっており、信頼されています。");
                        subjectName = cert.Subject;
                    }
                    else
                    {
                        // 間違っている場合
                        foreach (var status in chain.ChainStatus)
                        {
                            MessageBox.Show($"エラー：この証明書は不正です。");
                            MessageBox.Show($"理由：{status.StatusInformation}");

                        }
                        // throwは想定外の異常に使うものなのでreturnを使う
                        // throw new Exception("終了します");
                        return;
                    }
                }

                // 2. SHA256ハッシュの計算
                string hashString;
                using (SHA256 sha256 = SHA256.Create())
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }

                // 3. 結果の保存
                string outputPath = filePath + ".sha256";
                File.WriteAllText(outputPath, $"{hashString}  {Path.GetFileName(filePath)}");

                // 成功したらメッセージを出す
                MessageBox.Show($"検証成功：{subjectName}\nハッシュを保存しました。", "完了");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"警告：{ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
