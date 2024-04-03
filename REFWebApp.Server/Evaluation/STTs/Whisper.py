import csv
import whisper
import librosa
import csv
import boto3
import json
import io

#pip uninstall whisper
#pip install openai-whisper

file_paths = ["EV1-MAURER-2021-04-02_08-37-06-003/0_2_1-0_2_5.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_4-0_2_7.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_23-0_2_25.wav"]


def runWhisper(file_content):

    # with open(audiofilename, "rb") as audio_file:
    #     content = audio_file.read()

    model = whisper.load_model("base")
    result = model.transcribe(audio=file_content)
    return result["text"]

def transcribe_one(file): 
    client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")
    response = client.get_object(Bucket='nbl-audio-files', Key=file.strip(),)
    data = response["Body"].read()
    audio_io = io.BytesIO(data)
    audio_array, sample_rate = librosa.load(io.BytesIO(data), sr=None, mono=True)
    transcript = runWhisper(audio_array)
    print(transcript)
    return transcript


def transcribe_all(files_dir): 
    wavfiles = files_dir
    client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")

    transcript_dict = {}
    for i in range(len(wavfiles)): 
        response = client.get_object(Bucket='nbl-audio-files', Key=wavfiles[i],)
        data = response["Body"].read()
        audio_array, sample_rate = librosa.load(io.BytesIO(data), sr=None, mono=True)

        transcript = runWhisper(audio_array)
        transcript_dict[wavfiles[i]] = transcript
        print(transcript)
        with open('transcriptions.csv', 'w') as csv_file:  
            writer = csv.writer(csv_file)
            for key, value in transcript_dict.items():
                writer.writerow([key.replace('_', ':'), value])  

    print(transcript_dict)
    return transcript_dict.values()

# file_paths = ["EV1-MAURER-2021-04-02_08-37-06-003/0_39_18-0_39_21.wav"]
# transcribe_all(file_paths)