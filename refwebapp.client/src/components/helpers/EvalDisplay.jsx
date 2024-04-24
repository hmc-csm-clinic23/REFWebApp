import { React, useState } from "react";
import { Evals } from "../.././components";
function EvalDisplay({ stt, refData, scenarios }) {
  const [search, setSearch] = useState("");
  return (
    <div>
      <h1>{stt}</h1>
        <div className="importantScores">
          <h2>Total Score: {(refData.map(data => data.totalScore).reduce((x, y) => x + y, 0) / refData.map(data => data.totalScore).length).toFixed(2)}</h2>
          <h2>Accuracy: {(refData.map(data => data.accuracy).reduce((x, y) => x + y, 0) / refData.map(data => data.accuracy).length).toFixed(2)}</h2>
          <h2>Speed: {(refData.map(data => data.speed).reduce((x, y) => x + y, 0) / refData.map(data => data.speed).length).toFixed(0)}s</h2>
        </div>

      <input
        type="text"
        className="searchAudio"
        placeholder="Search scenarios"
        onChange={(e) => setSearch(e.target.value)}
      />
      <div className="evalContainer">
        <div className="evalItem">
          <div className="evalText">Metrics</div>
          <div className="evalText">Transcription</div>
          <div className="evalText">Ground Truth</div>
          <div className="evalText">Audio</div>
        </div>
        {refData
        .map((data, i) => ({ data, i }))
        .filter(({ i }) =>
          search.toLowerCase() === ""
            ? scenarios[i]
            : scenarios[i].name.toLowerCase().includes(search.toLowerCase()),
        )
        .map(({ data, i }) => 
        <Evals
        key={i}
        wer={data.wer}
        mer={data.mer}
        wil={data.wil}
        sim={data.sim}
        dist={data.dist}
        transcriptions={data.transcriptions}
        groundTruths={data.groundTruths}
        audioFiles={data.paths}
        scenarios={scenarios[i]}
        />
        )}
      </div>
    </div>
  );
}

export default EvalDisplay;
