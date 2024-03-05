from os import listdir
from os.path import isfile, join
import argparse
import librosa
import csv

from google.cloud import speech


import boto3
import json
import io

# access key: AKIA3XQL3GCMUFJ3ILUV
# secret: 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
# region: us-west-1


file_paths = ["EV1-MAURER-2021-04-02_08-37-06-003/0_2_1-0_2_5.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_4-0_2_7.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_23-0_2_25.wav"]

# file_paths = ["/Users/sathv/REFWebApp/REFWebApp.Server/Model/test.wav"]

# audio_segments_path_0 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-000'
# audio_segments_path_1 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-001'
# audio_segments_path_2 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-002'
# audio_segments_path_3 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-003'

# wav_file = "/Users/sathv/REFWebApp/REFWebApp.Server/Model/test.wav"
# client = speech.SpeechClient()
# with open(wav_file, "rb") as audio_file:
#     content = audio_file.read()
# print(content) 
# CURRENTPATH = audio_segments_path_0


def transcribe_file(file_content) -> speech.RecognizeResponse:
    #print(wav_file)
    client = speech.SpeechClient()
    # with open(wav_file, "rb") as audio_file:
    #     content = audio_file.read()
    # print(content) 

    audio = speech.RecognitionAudio(content=file_content)
    config = speech.RecognitionConfig(
        encoding=speech.RecognitionConfig.AudioEncoding.LINEAR16,
        sample_rate_hertz=16000,
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
    client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")
    # response = client.get_object(Bucket='nbl-audio-files', Key=files_dir[1],)
    # data = response["Body"].read()
    # print(data)

    transcript_dict = {}
    for i in range(len(file_paths)): 
        # print(i)
        # if librosa.get_duration(path = join(CURRENTPATH, i)) < 60:
        #     transcript_dict[i] = transcribe_file(join(CURRENTPATH, i))
        #     print(transcript_dict[i])
        #if librosa.get_duration(i) < 60:
        response = client.get_object(Bucket='nbl-audio-files', Key=file_paths[i],)
        data = response["Body"].read()
        # print(type(data))
        # print(data)
        transcript = transcribe_file(data)
        transcript_dict[file_paths[i]] = transcript
        print(transcript)
        #print(transcript_dict[i])
        with open('transcriptions.csv', 'w') as csv_file:  
            writer = csv.writer(csv_file)
            for key, value in transcript_dict.items():
                writer.writerow([key.replace('_', ':'), value])  

    return transcript_dict

transcribe_all(file_paths)