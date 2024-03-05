import csv
import whisper


import boto3
import json
import io

file_paths = ["EV1-MAURER-2021-04-02_08-37-06-003/0_2_1-0_2_5.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_4-0_2_7.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_23-0_2_25.wav"]


def runWhisper(file_content):

    # with open(audiofilename, "rb") as audio_file:
    #     content = audio_file.read()

    model = whisper.load_model("base")
    result = model.transcribe(audio=file_content)
    return result["text"]


def transcribe_all(files_dir): 
    wavfiles = files_dir
    client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")
    # response = client.get_object(Bucket='nbl-audio-files', Key=files_dir[1],)
    # data = response["Body"].read()
    # print(data)

    transcript_dict = {}
    for i in range(len(file_paths)): 
    #wavfiles = listdir(CURRENTPATH)
        response = client.get_object(Bucket='nbl-audio-files', Key=file_paths[i],)
        data = response["Body"].read()
            # print(type(data))
            # print(data)
        transcript = runWhisper(data)
        transcript_dict[file_paths[i]] = transcript
        print(transcript)
        #print(transcript_dict[i])
        with open('transcriptions.csv', 'w') as csv_file:  
            writer = csv.writer(csv_file)
            for key, value in transcript_dict.items():
                writer.writerow([key.replace('_', ':'), value])  

    return transcript_dict