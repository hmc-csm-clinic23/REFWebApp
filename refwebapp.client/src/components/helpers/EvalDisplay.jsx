import { React, useState } from "react";
import { Evals } from "../.././components";
function EvalDisplay({ stt, metrics, transcriptions, groundTruths, scenarios }) {
  const metricsList = metrics.reduceRight((all, item) => ({[item]: all}), {});
  const transcriptionsList = transcriptions.reduceRight((all, item) => ({[item]: all}), {});
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
        {metrics
        .map((metrics, i) => ({ metrics, i }))
        .filter(({ i }) =>
          search.toLowerCase() === ""
            ? scenarios[i]
            : scenarios[i].name.toLowerCase().includes(search.toLowerCase()),
        )
        .map(({ metrics, i }) => 
        <Evals
        metrics={metrics}
        transcriptions={transcriptions[i]}
        groundTruths={groundTruths[i]}
        scenarios={scenarios[i]}
        />
        )}
      </div>
    </div>
  );
}

export default EvalDisplay;
