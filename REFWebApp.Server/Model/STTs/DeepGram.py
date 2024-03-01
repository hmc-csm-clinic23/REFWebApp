from deepgram import DeepgramClient, PrerecordedOptions

# The API key we created in step 3
DEEPGRAM_API_KEY = '45b4ab38dc898ee8ea4b237e8edda5747a1288e4'

def runDeepgram(filepath):
    deepgram = DeepgramClient(DEEPGRAM_API_KEY)

    with open(filepath, 'rb') as buffer_data:
        payload = { 'buffer': buffer_data }

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
    wavfiles = files_dir
    transcript_dict = {}
    for i in wavfiles: 
        print(i)
        transcript_dict[i] = runDeepgram(i)
        
    return transcript_dict
