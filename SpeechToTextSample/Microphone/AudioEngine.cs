namespace SpeechToTextSample.Microphone
{
    using System;
    using AVFoundation;
    using Foundation;
    using Speech;

    public class AudioEngine
    {
        private static readonly nuint BusNumber = 0;
        private static readonly uint BufferSize = 1024;

        private AVAudioEngine engine = new AVAudioEngine();
        private SFSpeechAudioBufferRecognitionRequest request;

        public SFSpeechAudioBufferRecognitionRequest StartAndReturnRequest()
        {
            request = new SFSpeechAudioBufferRecognitionRequest();
            InstallTapOnBusToInputNode();
            var error = StartAndReturnError();
            if (error != null)
            {
                throw new Exception(error.LocalizedDescription);
            }

            return request;
        }

        public void Stop()
        {
            engine.Stop();
            engine.InputNode.RemoveTapOnBus(BusNumber);

            if (request != null)
            {
                request.EndAudio();
                request = null;
            }
        }

        private void InstallTapOnBusToInputNode()
        {
            var inputNode = engine.InputNode;
            var recordingFormat = inputNode.GetBusOutputFormat(BusNumber);
            inputNode.InstallTapOnBus(BusNumber, BufferSize, recordingFormat, (buffer, when) =>
            {
                request.Append(buffer);
            });
        }

        private NSError StartAndReturnError()
        {
            engine.Prepare();
            NSError error = null;
            engine.StartAndReturnError(out error);
            return error;
        }
    }
}
