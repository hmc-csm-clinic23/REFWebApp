import { React } from "react";

function AudioList({ groundTruth, transcription, wer, mer, wil, sim, dist, audioFile }) {
  return (
    <li className="audioList">
      <div className="evalsText">{groundTruth}</div>
      <div className="evalsText">{transcription}</div>
      <div className="evalsText">{wer.toFixed(2)}</div>
      <div className="evalsText">{mer.toFixed(2)}</div>
      <div className="evalsText">{wil.toFixed(2)}</div>
      <div className="evalsText">{sim.toFixed(2)}</div>
      <div className="evalsText">{dist.toFixed(0)}</div>
      <audio controls src={audioFile} className="audioPlayer"></audio>
    </li>
  );
}

export default AudioList;
