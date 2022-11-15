using System;
using System.Threading;

namespace mutex_netframework.cs
{
  internal class Program
  {
    [STAThread]
    static int Main()
    {
      Thread.Sleep(1000);

      // Mutex名を決める（必ずアプリケーション固有の文字列に変更すること！）
      string mutexName = "MyApplicationName";
      // Mutexオブジェクトを作成する
      Mutex mutex = new Mutex(false, mutexName);

      bool hasHandle = false;
      try
      {
        try
        {
          // ミューテックスの所有権を要求する
          hasHandle = mutex.WaitOne(0, false);
        }

        catch (AbandonedMutexException)
        {
          // 別のアプリケーションがミューテックスを解放しないで終了した時
          hasHandle = true;
        }
        // ミューテックスを得られたか調べる
        if (hasHandle == false)
        {
          // 得られなかった場合は、すでに起動していると判断して終了
          Console.WriteLine("多重起動はできません。");
          Console.ReadKey();
          return 1;
        }

        Console.WriteLine("実行開始");
        Console.ReadKey();
        return 0;

      }
      finally
      {
        if (hasHandle)
        {
          // ミューテックスを解放する
          mutex.ReleaseMutex();
        }
        mutex.Close();
      }
    }
  }
}
