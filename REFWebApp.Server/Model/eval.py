# !pip3 install python-Levenshtein
# %pip install jiwer
# %pip install nest-asyncio
# !pip3 install pydub
import nest_asyncio
nest_asyncio.apply()

import pandas as pd
import csv
from num2words import num2words

# csv_0 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-000.csv'
# csv_1 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-001.csv'
# csv_2 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-002.csv'
# csv_3 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-003.csv'

def evaluate(list):
    transcription_file = list[0]
   
    groundtruth_file = list[1]

    df = pd.read_csv(groundtruth_file)
    print(df)
    
    from difflib import SequenceMatcher
    from jiwer import wer, mer, wil
    from Levenshtein import distance

    def metrics(asr, ground_truth : str, transcr : str):
        m_wer = round(wer(ground_truth, transcr),2)
        m_mer = round(mer(ground_truth, transcr),2)
        m_wil = round(wil(ground_truth, transcr),2)
        m_sim = round(SequenceMatcher(None, ground_truth, transcr).ratio(),2)
        m_dist = round(distance(transcr, ground_truth),2)
        print(asr, '-','WER:',m_wer, 'MER:',m_mer,'WIL:',m_wil,'SIM:',m_sim,'L-DIST:',m_dist)
        print(m_wer, m_mer, m_wil, m_sim, m_dist)
        return m_wer, m_mer, m_wil, m_sim, m_dist

    def avg(metrics):
        return str(round(sum(metrics) / len(metrics), 2))

    colnames = ['Wav', 'Transcription']
    transcript_dict = pd.read_csv(transcription_file, names=colnames )
    transcript_dict.dropna(subset=['Transcription'], inplace=True)
    

    def evaluate_transcr(transcripts: pd.DataFrame, groundtruthdf):
        #lists to contain metrics for both ASR scores
        # groundtruthdict = groundtruthdf.groupby('Wav',group_keys=True)[['Transcription']].apply(list).to_dict()
        groundtruthdict = dict(zip(groundtruthdf.Wav, groundtruthdf.Transcription))
        # transcriptdict = transcripts.groupby('Wav',group_keys=True)[['Transcription']].apply(list).to.dict()
        transcriptdict = dict(zip(transcripts.Wav, transcripts.Transcription))

        print(groundtruthdict)
        #print(transcriptdict)
        model_wer = []
        model_mer = []
        model_wil = []
        model_sim = []
        model_dist = []

        for key in groundtruthdict.keys():
            print(key)

            if key in transcriptdict.keys():
                groundtruth = groundtruthdict[key]
                model_transcription_text = transcriptdict[key]

                print('Ground Truth:', groundtruth, '\n' )
                print('Model Transcription:', model_transcription_text, '\n')

                wer, mer, wil, sim, dist = metrics('Model', groundtruth, model_transcription_text)
                
                model_wer.append(wer)
                model_mer.append(mer)
                model_wil.append(wil)
                model_sim.append(sim)
                model_dist.append(dist)
                print('\n')


        #print overall ASR metrics
        print('Average ASR Metrics - WER:', avg(model_wer), 'MER:', avg(model_mer), 'WIL:', avg(model_wil), 'SIM:', avg(model_sim), 'L-DIST:', avg(model_dist), '\n')
        model_metrics = {'wer': float(avg(model_wer)), 'mer': float(avg(model_mer)), 'wil' : float(avg(model_wil)), 'sim' : float(avg(model_sim)), 'l_dist' : float(avg(model_dist))}
        return model_metrics


    evaluate_transcr(transcript_dict, df)

def normalize(result):
  newResult = []
  for word in result.split():
    if word.isdigit():
      newResult.append(num2words(word))
    else:
      newResult.append(word)
  return (' '.join(newResult))