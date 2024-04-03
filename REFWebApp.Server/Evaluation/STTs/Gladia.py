import requests
import os
import soundfile as sf
import boto3
import librosa
import csv
import io

headers = {
   'x-gladia-key': 'bf095bf7-6630-4600-a6fe-fc91e333eb30	', # Replace with your Gladia Token
   'accept': 'application/json', # Accept json as a response, but we are sending a Multipart FormData
}


def runGladia(filepath, audio):

  file_name, file_extension = os.path.splitext(filepath) # Get your audio file name + extension
  files = {
    'audio': (file_name, audio, 'audio/'+file_extension[1:]),
    'toggle_diarization': (None, True),
  }
  response = requests.post('https://api.gladia.io/audio/text/audio-transcription/', headers=headers, files=files)
  if response.status_code == 200:
    result = response.json()
    return gladiaTranscript(result)
  else:
    print('- Request failed');
    print(response.json())

def gladiaTranscript(response):
  result = ""
  prediction = response["prediction"]
  for i in range(len(prediction)):
    for j in range(len(prediction[i]["words"])):
      result += prediction[i]["words"][j]["word"]
  return result

def transcribe_one(file): 
    client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")
    response = client.get_object(Bucket='nbl-audio-files', Key=file.strip(),)
    data = response["Body"].read()
    audio_io = io.BytesIO(data)
    pcm, samplerate = sf.read(audio_io)
    transcript = runGladia(file, data)
    print(transcript)
    return transcript

def transcribe_all(files_dir): 
  
    wavfiles = list(files_dir)
    client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")

    transcript_dict = {}
    for i in range(len(wavfiles)): 
        print(wavfiles[i])
        response = client.get_object(Bucket='nbl-audio-files', Key=wavfiles[i].strip(),)
        data = response["Body"].read()
        audio_io = io.BytesIO(data)
        pcm, samplerate = sf.read(audio_io)
        transcript_dict[i] = runGladia(wavfiles[i], data)
 
    print(transcript_dict)
    return transcript_dict.values()

# file_paths = ["EV1-MAURER-2021-04-02_08-37-06-003/0_2_4-0_2_7.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_23-0_2_25.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_26-0_2_31.wav"]
# transcribe_all(file_paths)