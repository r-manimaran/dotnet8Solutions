## Cancellation Token in Web Api
- HttpRequest pipeline will create a Cancellation Source Token and pass on.
- When the user navigates/ cancel to the page where the long running operation is happening, this will initiate the Cancellation Source Token cancel method and stops the long running process and releases the process.


1. Avoid cancelling operation after side-effects.
  - In the FileUpload example, if the process was cancelled after the file was uploaded, but before the Additonal tasks to be performed, this will cancel that additional operation. But the file will be uploaded orphan.
- Not all work is cancellable.
2. Make Token optional on public API and requried elsewhere.
3. Use CancellationToken.None after point of 'no cancellation'
4. Check CancellationToken.CanBeCanceled
5. Ignore CancellationToken if work is very quick.