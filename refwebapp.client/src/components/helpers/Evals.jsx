import { React, useState } from "react";
function Evals({ wer, mer, wil, sim, dist, transcriptions, groundTruths, scenarios }) {
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
      <div className="evalItem" key={i}>
        <div className="evalText">[{`WER: ${wer[i].toFixed(2)}, MER: ${mer[i].toFixed(2)}, WIL: ${wil[i].toFixed(2)}, Similarity: ${sim[i].toFixed(2)}, Levenshtein-Distance: ${dist[i].toFixed(2)}`}]</div>
        <div className="evalText">{transcriptions[i]}</div>
        <div className="evalText">{groundTruths}</div>
      </div>
      )}
    </div>
  );
}

export default Evals;
