
var source = new CancellationTokenSource();

Console.WriteLine("Press any key to cancel the Operation!..");
var longRunningTask = LongRunningOperationAsync(source.Token);

Console.ReadKey();
Console.WriteLine("Key Pressed");
// If any key is pressed, set the Source to Cancel
source.Cancel();

await longRunningTask;


async Task LongRunningOperationAsync(CancellationToken cancellationToken)
{
   var i=0;
 try
 {
      while(i++ < 10)
      {
         cancellationToken.ThrowIfCancellationRequested();
         Console.WriteLine($"Long running operation is started running Count:{i}...");
         await Task.Delay(2000,cancellationToken);
         Console.WriteLine($"Long running operation is completed Count:{i}...");
      }
 }
 catch (OperationCanceledException e)
 {
    Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
    throw;
 }
}