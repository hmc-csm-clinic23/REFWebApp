import boto3
import time
import pandas as pd
import csv

# transcribe = boto3.client('transcribe',
#                           aws_access_key_id = "AKIAZI2LDMYOI5O3ENFU",
#                           aws_secret_access_key = "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",

transcribe = boto3.client('transcribe',
                          aws_access_key_id = "AKIAZI2LDMYOI5O3ENFU",
                          aws_secret_access_key = "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",
                          region_name = "us-east-2")

bucket_name = 'nbl-audio-files'
file_paths = ["EV1-MAURER-2021-04-02_08-37-06-003/0_2_1-0_2_5.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_4-0_2_7.wav", "EV1-MAURER-2021-04-02_08-37-06-003/0_2_23-0_2_25.wav"]

def check_job_name(job_name):
  job_verification = True

  # all the transcriptions
  existed_jobs = transcribe.list_transcription_jobs()

  for job in existed_jobs['TranscriptionJobSummaries']:
    if job_name == job['TranscriptionJobName']:
      job_verification = False
      break

  if job_verification == False:
      transcribe.delete_transcription_job(TranscriptionJobName=job_name)
  return job_name

def runAmazon(job_uri, audio_file_name):
  # Usually, file names have spaces and have the file extension like .mp3
  # we take only a file name and delete all the space to name the job
  job_name = (audio_file_name.split('.')[0]).replace(" ", "")

  # file format
  file_format = audio_file_name.split('.')[1]
  print("AMAZON HERE")
  # check if name is taken or not
  job_name = check_job_name(job_name)
  transcribe.start_transcription_job(
      TranscriptionJobName=job_name,
      Media={'MediaFileUri': job_uri},
      MediaFormat = file_format,
      LanguageCode='en-US')

  while True:
    result = transcribe.get_transcription_job(TranscriptionJobName=job_name)
    if result['TranscriptionJob']['TranscriptionJobStatus'] in ['COMPLETED', 'FAILED']:
      break
    time.sleep(15)

  if result['TranscriptionJob']['TranscriptionJobStatus'] == "COMPLETED":
    data = pd.read_json(result['TranscriptionJob']['Transcript']['TranscriptFileUri'])

  return data['results'][1][0]['transcript']

def transcribe_all(files_dir): 
    wavfiles = files_dir

    # client = boto3.client('s3',aws_access_key_id = "AKIA3XQL3GCMUFJ3ILUV", aws_secret_access_key = "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", region_name = "us-west-1")
    client = boto3.client('transcribe',
                          aws_access_key_id = "AKIAZI2LDMYOI5O3ENFU",
                          aws_secret_access_key = "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",
                          region_name = "us-east-2")

    transcript_dict = {}
    for i in range(len(file_paths)): 
        job_uri = "s3://transcript-yin/EV1-MAURER-2021-04-02_08-37-06-000/0_1_5-0_1_8.wav"
        audio_file_name = file_paths[i].split('/')[-1]
        print(job_uri)
        transcript = runAmazon(job_uri, audio_file_name)
        transcript_dict[file_paths[i]] = transcript
        print(transcript)
        with open('transcriptions.csv', 'w') as csv_file:  
            writer = csv.writer(csv_file)
            for key, value in transcript_dict.items():
                writer.writerow([key.replace('_', ':'), value])  

    return transcript_dict

transcribe_all(file_paths)