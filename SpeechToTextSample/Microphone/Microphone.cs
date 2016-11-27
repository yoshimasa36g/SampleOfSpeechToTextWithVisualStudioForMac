namespace SpeechToTextSample.Microphone
{
    using System;
    using AVFoundation;
    using Foundation;
    using Speech;

    public class Microphone
    {
        private AudioEngine engine = new AudioEngine();
        private SpeechRecognitionTask recognitionTask;

        public void On(Action<SFSpeechRecognitionResult, NSError> handler)
        {
            var request = engine.StartAndReturnRequest();
            recognitionTask = SpeechRecognitionTask.StartAndReturnTask(request, handler);
        }

        public void Off()
        {
            engine.Stop();
            recognitionTask = null;
        }
    }
}
