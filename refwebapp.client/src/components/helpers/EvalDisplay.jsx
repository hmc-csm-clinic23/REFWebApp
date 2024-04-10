import { React, useState } from "react";
import { Evals } from "../.././components";
function EvalDisplay({ stt, refData, scenarios }) {
  const [search, setSearch] = useState("");
  return (
    <div>
      <h1>{stt}</h1>
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
