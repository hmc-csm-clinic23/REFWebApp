import { React, useState } from "react";
function Evals({ metrics, transcriptions, groundTruths, scenarios }) {
  const metricLabels = ["WER: ", "MER: ", "WIL: ", "SIM: ", "L-DIST: "];
  const [search, setSearch] = useState("");
  return (
    <div>
      <div className="evalItem">
        <div className="evalScenarioText"></div>
        <div className="evalScenarioText">{scenarios.name}</div>
        <div className="evalScenarioText"></div>
      </div>
      <div className="evalSearchItem">
        <input
          type="text"
          className="evalText"
          placeholder="Search ground truths"
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>
      {groundTruths
      .map((groundTruths, i) => ({ groundTruths, i }))
      .filter(({ groundTruths }) =>
        search.toLowerCase() === ""
          ? groundTruths
          : groundTruths.toLowerCase().includes(search.toLowerCase()),
      )
      .map(({ groundTruths, i }) => 
      <div className="evalItem">
        <div className="evalText">[{metrics[i].map((metric, index) => [metricLabels[index], metric, index < 4 && ", "])}]</div>
        <div className="evalText">{transcriptions[i]}</div>
        <div className="evalText">{groundTruths}</div>
      </div>
      )}
    </div>
  );
}

export default Evals;
