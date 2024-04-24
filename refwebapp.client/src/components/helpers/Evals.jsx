import { React, useState } from "react";
import { Eval } from "../.././components";
function Evals({ wer, mer, wil, sim, dist, transcriptions, groundTruths, audioFiles, scenarios }) {
  const [search, setSearch] = useState("");
  return (
    <div>
      <div className="evalScenarioItem">
        <div className="evalScenarioText">{scenarios.name}</div>
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
      <Eval
      key={i}
      wer={wer[i]}
      mer={mer[i]}
      wil={wil[i]}
      sim={sim[i]}
      dist={dist[i]}
      transcriptions={transcriptions[i]}
      groundTruths={groundTruths}
      audioFile={audioFiles[i]}
      />
      )}
    </div>
  );
}

export default Evals;
