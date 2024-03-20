import boto3
import csv
from deepgram import DeepgramClient, PrerecordedOptions

# The API key we created in step 3
DEEPGRAM_API_KEY = '45b4ab38dc898ee8ea4b237e8edda5747a1288e4'
file_paths = ["EV1-MAURER-2021-04-02_08-37-06-000/0_1_5-0_1_8.wav"]

def runDeepgram(filepath):
    deepgram = DeepgramClient(DEEPGRAM_API_KEY)

    payload = { 'buffer': filepath }

    options = PrerecordedOptions(
        smart_format=True, model="nova-2", language="en-US"
    )
    response = deepgram.listen.prerecorded.v('1').transcribe_file(payload, options)
    return dgTranscript(response)

def dgTranscript(response):
    channels = response['results'].channels
    tempchannel = channels[0]
    alternative = tempchannel.alternatives
    tempalternative = alternative[0]
    result = tempalternative.transcript
    return result


def transcribe_all(files_dir): 
    client = boto3.client('s3',
                          aws_access_key_id = "AKIAZI2LDMYOI5O3ENFU",
                          aws_secret_access_key = "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",
                          region_name = "us-east-2")
    transcript_dict = {}
    for i in range(len(file_paths)): 
        response = client.get_object(Bucket='transcript-yin', Key=file_paths[i],)
        data = response["Body"].read()
        transcript = runDeepgram(data)
        transcript_dict[file_paths[i]] = transcript
        with open('transcriptions.csv', 'w') as csv_file:  
            writer = csv.writer(csv_file)
            for key, value in transcript_dict.items():
                writer.writerow([key.replace('_', ':'), value])
        
    return transcript_dict

transcribe_all(file_paths)