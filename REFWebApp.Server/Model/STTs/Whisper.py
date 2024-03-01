import whisper



def runWhisper(audiofilename):

    with open(audiofilename, "rb") as audio_file:
        content = audio_file.read()

    model = whisper.load_model("base")
    result = model.transcribe(audio=content)
    return result["text"]


def transcribe_all(files_dir): 
    
    #wavfiles = listdir(CURRENTPATH)
    wavfiles = files_dir
    transcript_dict = {}
    for i in wavfiles: 
        print(i)
        transcript_dict[i] = runWhisper(i)
    return transcript_dict