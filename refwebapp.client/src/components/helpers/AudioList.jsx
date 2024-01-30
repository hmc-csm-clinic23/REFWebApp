import { React } from "react";

function AudioList({ key, name, groundTruth, transcription, wer, audioFile }) {
  return (
    <li className="audioList">
      <div className="evalsText">{name}</div>
      <div className="evalsText">{groundTruth}</div>
      <div className="evalsText">{transcription}</div>
      <div className="evalsText">{wer}</div>
      <audio controls src={audioFile} className="audioPlayer"></audio>
    </li>
  );
}

export default AudioList;
