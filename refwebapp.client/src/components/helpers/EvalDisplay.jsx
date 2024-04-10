import { React, useState } from "react";
import { Evals } from "../.././components";
function EvalDisplay({ stt, refData, scenarios }) {
  const [search, setSearch] = useState("");
  return (
    <div>
      <h1>{stt}</h1>
          <h2>Total Score: {(refData.map(data => data.totalScore).reduce((x, y) => x + y, 0) / refData.map(data => data.totalScore).length).toFixed(2)}</h2>
          <h2>Accuracy: {(refData.map(data => data.accuracy).reduce((x, y) => x + y, 0) / refData.map(data => data.accuracy).length).toFixed(2)}</h2>
      <input
        type="text"
        className="searchAudio"
        placeholder="Search scenarios"
        onChange={(e) => setSearch(e.target.value)}
      />
      <div className="evalContainer">
        <div className="evalItem">
          <div className="evalText">Metrics</div>
          <div className="evalText">Transcriptions</div>
          <div className="evalText">Ground Truths</div>
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
        wer={data.wer}
        mer={data.mer}
        wil={data.wil}
        sim={data.sim}
        dist={data.dist}
        transcriptions={data.transcriptions}
        groundTruths={data.groundTruths}
        scenarios={scenarios[i]}
        />
        )}
      </div>
    </div>
  );
}

export default EvalDisplay;
