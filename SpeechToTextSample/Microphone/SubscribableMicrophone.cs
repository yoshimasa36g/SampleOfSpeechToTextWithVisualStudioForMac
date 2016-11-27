namespace SpeechToTextSample.Microphone
{
    using System;
    using System.Reactive.Subjects;
    using Foundation;
    using Speech;

    public class SubscribableMicrophone
    {
        private Microphone audioEngine = new Microphone();
        private Subject<string> subject = new Subject<string>();

        public IDisposable Subscribe(Action<string> onNext, Action<Exception> onError, Action onCompleted)
        {
            return subject.Subscribe(onNext, onError, onCompleted);
        }

        public void On()
        {
            try
            {
                audioEngine.On(HandleSpeechResult);
            }
            catch (Exception exception)
            {
                subject.OnError(exception);
            }
        }

        public void Off()
        {
            audioEngine.Off();
        }

        private void NoticeError(string errorMessage)
        {
            Off();
            subject.OnError(new Exception(errorMessage));
        }

        private void HandleSpeechResult(SFSpeechRecognitionResult result, NSError error)
        {
            if (error != null)
            {
                NoticeError(error.LocalizedDescription);
                return;
            }

            if (result.Final)
            {
                subject.OnNext(result.BestTranscription.FormattedString);
            }
        }
    }
}
