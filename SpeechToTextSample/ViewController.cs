
namespace SpeechToTextSample
{
    using System;
    using AVFoundation;
    using CoreGraphics;
    using Microphone;
    using Speech;
    using UIKit;

    public partial class ViewController : UIViewController
    {
        private SubscribableMicrophone microphone = new SubscribableMicrophone();
        private IDisposable disposable;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            var button = CreateSpeechButton();
            View.AddSubview(button);
            var label = CreateResultLabel();
            View.AddSubview(label);
            RequestSpeechAuthorization(button);
            disposable = microphone.Subscribe(
                (string text) => InvokeOnMainThread(() => label.Text = text),
                (Exception exception) => Console.WriteLine(exception.Message),
                () => disposable.Dispose());
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void RequestSpeechAuthorization(UIButton button)
        {
            if (SFSpeechRecognizer.AuthorizationStatus == SFSpeechRecognizerAuthorizationStatus.Authorized)
            {
                return;
            }

            button.Enabled = false;

            SFSpeechRecognizer.RequestAuthorization((status) =>
            {
                if (status == SFSpeechRecognizerAuthorizationStatus.Authorized)
                {
                    InvokeOnMainThread(() => button.Enabled = true);
                }
            });
        }

        private UIButton CreateSpeechButton()
        {
            var button = UIButton.FromType(UIButtonType.Custom);
            button.Frame = new CGRect(0, 0, 48, 48);
            button.SetImage(UIImage.FromBundle("microphone"), UIControlState.Normal);
            button.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            button.Center = new CGPoint(this.View.Bounds.Width / 2, this.View.Bounds.Height - 50);
            button.TouchDown += (sender, e) =>  microphone.On();
            button.TouchUpInside += (sender, e) => microphone.Off();
            return button;
        }

        private UILabel CreateResultLabel()
        {
            var label = new UILabel();
            label.Frame = new CGRect(0, 0, this.View.Bounds.Width, 48);
            label.Center = new CGPoint(this.View.Bounds.Width / 2, this.View.Bounds.Height / 2);
            label.TextAlignment = UITextAlignment.Center;
            return label;
        }
    }
}
