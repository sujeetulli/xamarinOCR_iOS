using System;
using Foundation;
using Tesseract;
using UIKit;

namespace XamarinOCR
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			OCRCommand.TouchDown += delegate {
				LoadOCR();
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		async void LoadOCR()
		{
			ITesseractApi _tesseract = new TesseractApi();
			if (!_tesseract.Initialized)
			{
				///OcrEngineMode
				//TesseractOnly Run Tesseract only - fastest  - by defaylt.
				///CubeOnly- Run Cube only - better accuracy, but slower
				///TesseractCubeCombined -Run both and combine results - best accuracy
				var initialised = await _tesseract.Init("eng");
				if (!initialised)
					return;
			}

			//_tesseract.SetWhitelist("0123456789");
			var image = UIImage.FromBundle("ocrTestImage");
			Byte[] myByteArray = null;
			using (NSData imageData = image.AsPNG())
			{
				myByteArray = new Byte[imageData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, myByteArray, 0, Convert.ToInt32(imageData.Length));
			}

			var result = _tesseract.SetImage(myByteArray).ContinueWith((t) =>
			{
				BeginInvokeOnMainThread(() =>
				{
					var resultStr = _tesseract.Text;
					OCRTextResult.Text = resultStr;
					//var words = _tesseract.Results(PageIteratorLevel.Word);
					//var symbols = _tesseract.Results(PageIteratorLevel.Symbol);
					//var blocks = _tesseract.Results(PageIteratorLevel.Block);
					//var paragraphs = _tesseract.Results(PageIteratorLevel.Paragraph);
					//var lines = _tesseract.Results(PageIteratorLevel.Textline);
				});
			});
		}
	}
}
