import boto3
import time
import pandas as pd

transcribe = boto3.client('transcribe',
                          aws_access_key_id = "AKIAZI2LDMYOI5O3ENFU",
                          aws_secret_access_key = "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",
                          region_name = "us-east-2")

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
    transcript_dict = {}
    for i in wavfiles: 
        transcript_dict[i] = runAmazon(i)

    return transcript_dict