namespace SpeechToTextSample.Microphone
{
    using System;
    using Foundation;
    using Speech;

    public class SpeechRecognitionTask
    {
        private static readonly string Locale = "en_US";

        private SFSpeechRecognitionTask task;

        private SpeechRecognitionTask(SFSpeechRecognitionTask task)
        {
            this.task = task;
        }

        public static SpeechRecognitionTask StartAndReturnTask(
            SFSpeechAudioBufferRecognitionRequest request, 
            Action<SFSpeechRecognitionResult, NSError> handler)
        {
            var speechRecognizer = new SFSpeechRecognizer(new NSLocale(Locale));
            return new SpeechRecognitionTask(speechRecognizer.GetRecognitionTask(request, handler));
        }

        public void CancelIfStartingOrRunning()
        {
            switch (task.State)
            {
                case SFSpeechRecognitionTaskState.Running:
                case SFSpeechRecognitionTaskState.Starting:
                    task.Cancel();
                    break;
                default:
                    break;
            }
        }
    }
}
