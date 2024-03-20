import requests
import os

headers = {
   'x-gladia-key': 'bf095bf7-6630-4600-a6fe-fc91e333eb30	', # Replace with your Gladia Token
   'accept': 'application/json', # Accept json as a response, but we are sending a Multipart FormData
}


def runGladia(filepath):
  print(os.getcwd())

  if not os.path.exists(filepath): # This is here to check if the file exists
    print("- File does not exist")

  file_name, file_extension = os.path.splitext(filepath) # Get your audio file name + extension

  with open(filepath, 'rb') as f:
    files = {
      'audio': (file_name, f, 'audio/'+file_extension[1:]),
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


def transcribe_all(files_dir): 
    wavfiles = files_dir
    transcript_dict = {}
    for i in wavfiles: 
        print(i)
        transcript_dict[i] = runGladia(i)
    return transcript_dict

