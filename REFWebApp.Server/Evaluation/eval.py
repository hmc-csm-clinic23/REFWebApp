# !pip3 install python-Levenshtein
# %pip install jiwer
# %pip install nest-asyncio
# !pip3 install pydub
from asyncio.windows_events import NULL
import nest_asyncio
nest_asyncio.apply()

import pandas as pd
import csv

# csv_0 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-000.csv'
# csv_1 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-001.csv'
# csv_2 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-002.csv'
# csv_3 = '/Users/sathv/Desktop/NBL_GroundTruth_DataSet/EV1-MAURER-2021-04-02_08-37-06-003.csv'
# t = ["hello"]
# g = ["Hello!"]


def evaluate(transcriptionlist, groundtruthlist):
    transcriptions = transcriptionlist
   
    groundtruths = groundtruthlist

    # df = pd.asDataFrame(transcriptions)
    # print(df)
   
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

    def evaluate_transcr(transcripts, groundtruths):
        #lists to contain metrics for both ASR scores
        # groundtruthdict = groundtruthdf.groupby('Wav',group_keys=True)[['Transcription']].apply(list).to_dict()
        # groundtruthdict = dict(zip(groundtruthdf.Wav, groundtruthdf.Transcription))
        # transcriptdict = transcripts.groupby('Wav',group_keys=True)[['Transcription']].apply(list).to.dict()
        # transcriptdict = dict(zip(transcripts.Wav, transcripts.Transcription))

        print(groundtruths)
        print(transcriptions)
        model_wer = []
        model_mer = []
        model_wil = []
        model_sim = []
        model_dist = []

        for i in range(len(groundtruths)):
            # print(key)

            # if key in transcriptdict.keys():
            #run transcription function
            groundtruth = groundtruths[i]
            if groundtruth == NULL: 
                groundtruth = ''
            
            model_transcription_text = transcripts[i]

            #print(model_transcription_text)
        
            #print results from both ASRs

            print('Ground Truth:', groundtruth, '\n' )
            print('Model Transcription:', model_transcription_text, '\n')

            #print ASR metrics
            wer, mer, wil, sim, dist = metrics('Model', groundtruth, model_transcription_text)

            #append metrics to ASR score lists

            model_wer.append(wer)
            model_mer.append(mer)
            model_wil.append(wil)
            model_sim.append(sim)
            model_dist.append(dist)
            print('\n')


        #print overall ASR metrics
        print('Average ASR Metrics - WER:', avg(model_wer), 'MER:', avg(model_mer), 'WIL:', avg(model_wil), 'SIM:', avg(model_sim), 'L-DIST:', avg(model_dist), '\n')
        model_metrics = {'wer': float(avg(model_wer)), 'mer': float(avg(model_mer)), 'wil' : float(avg(model_wil)), 'sim' : float(avg(model_sim)), 'l_dist' : float(avg(model_dist))}
        metrics_avg_list = [float(avg(model_wer)), float(avg(model_mer)), float(avg(model_wil)), float(avg(model_sim)), float(avg(model_dist))]
        metrics_list = []
        for i in range(len(model_dist)):
            metrics_list.append([model_wer[i], model_mer[i], model_mer[i], model_sim[i], model_dist[i]])
        print('METRICS FROM PYTHON: ', metrics_list)
        return metrics_list

    metrics = evaluate_transcr(transcriptions, groundtruths)
    return metrics


# evaluate(t, g)