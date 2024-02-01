from os import listdir
from os.path import isfile, join
import argparse
import librosa
import csv
import boto3

from google.cloud import speech

audio_segments_path_0 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-000'
audio_segments_path_1 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-001'
audio_segments_path_2 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-002'
audio_segments_path_3 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-003'

CURRENTPATH = audio_segments_path_0


def transcribe_file(wav_file: str) -> speech.RecognizeResponse:
    #print(wav_file)
    client = speech.SpeechClient()
    with open(wav_file, "rb") as audio_file:
        content = audio_file.read()

    audio = speech.RecognitionAudio(content=content)
    config = speech.RecognitionConfig(
        encoding=speech.RecognitionConfig.AudioEncoding.LINEAR16,
        sample_rate_hertz=48000,
        language_code="en-US",
    )

    response = client.recognize(config=config, audio=audio)

    # Each result is for a consecutive portion of the audio. Iterate through
    # them to get the transcripts for the entire audio file.
    #for result in response.results:
        # The first alternative is the most likely one for this portion.
        #print(f"Transcript: {result.alternatives[0].transcript}")
        #print(f"Confidence: {result.alternatives[0].confidence}")
        #print(f"Full Result: {result}")
    recognized_text = ""
    for i in range(len(response.results)):
        recognized_text += response.results[i].alternatives[0].transcript
    #print(recognized_text)
    return recognized_text
#or write a similar function that uses a list of previous transcriptions to compare them to ground truth transcriptions


def transcribe_all(files_dir): 
    
    #wavfiles = listdir(CURRENTPATH)
    wavfiles = files_dir
    transcript_dict = {}
    for i in wavfiles: 
        print(i)
        # if librosa.get_duration(path = join(CURRENTPATH, i)) < 60:
        #     transcript_dict[i] = transcribe_file(join(CURRENTPATH, i))
        #     print(transcript_dict[i])
        #if librosa.get_duration(i) < 60:
        transcript_dict[i] = transcribe_file(i)
        #print(transcript_dict[i])
        # with open('transcriptions.csv', 'w') as csv_file:  
        #     writer = csv.writer(csv_file)
        #     for key, value in transcript_dict.items():
        #     writer.writerow([key.replace('_', ':'), value])  

    return transcript_dict