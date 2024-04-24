import { React, useState, useEffect } from "react";
import { RankingLine, ScenarioDropdown } from "../.././components";
import "./ranking.css";

function Ranking() {
  const [scenarioToggle, setScenarioToggle] = useState(false);
  const [scenarioList, setScenarioList] = useState([]);
  const [rankingList, setRankingList] = useState([]);
  const [scenarioSearch, setScenarioSearch] = useState("");
  const [sort, setSort] = useState("score");
  const [sortToggle, setSortToggle] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const scenarioSubmit = scenarioList.filter((scenario) => scenario.checked === true,).map(({checked, ...scenario}) => scenario).map(({weight, ...scenario}) => scenario);
  const scenarioWeights = scenarioList.filter((scenario) => scenario.checked === true,).map(({weight, ...scenario}) => weight);

  async function populateScenarioData() {
    const response = await fetch('scenariolist');
    const data = await response.json();
    setScenarioList(
        data.map((scenario) => ({
        ...scenario,
        checked: false,
        weight: 0,
      })),
    );
    }
  async function populateRankingData(signal) {
    try {
      const response = await fetch('rankings',
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            ScenarioList: scenarioSubmit,
            WeightList: scenarioWeights
        }),
        signal: signal
      });
      const data = await response.json();
      setRankingList(data);
      setError(null);
    } catch (error) {
        if (error.name === "AbortError") {
            console.log("Fetch aborted"); // Log a message when the request is intentionally aborted
            return; // Exit the function to prevent further error handling
        }
        setError(error.message);
    } finally {
        setLoading(false);
    }
  }

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `scenarios`
    // then set your scenarioList state
    populateScenarioData();
  }, []);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `rankings`
    // set a fetch buffer so there isn't constant fetching while the checks and weightsliders are changing
    // then set your rankingList state
    if (scenarioSubmit.length == 0) {
      return;
    }
    const controller = new AbortController();
    const signal = controller.signal;
    populateRankingData(signal);
    return () => {
      controller.abort(); // Cancel any ongoing requests
      setLoading(true); // Reset loading state
    };
  }, [scenarioList]);

  const updateSort = (field) => {
    setSort(field);
    setSortToggle(sort === field ? -sortToggle : 1);
  };

  function compare(a, b) {
      const bool = sort === "sttName" || sort === "speed" | sort === "wer" || sort === "mer" || sort === "wil" || sort === "sim" || sort === "dist";
    if (a[sort] < b[sort]) {
      return bool ? -sortToggle : sortToggle;
    }
    if (a[sort] > b[sort]) {
      return bool ? sortToggle : -sortToggle;
    }
    return 0;
  }

  return (
    <div className="rankingPage">
      <div className="buttonContainer">
        <ScenarioDropdown
          toggle={scenarioToggle}
          setToggle={setScenarioToggle}
          search={scenarioSearch}
          setSearch={setScenarioSearch}
          list={scenarioList}
          setList={setScenarioList}
        />
      </div>
      <div className="rankingContainer">
        <div className="rankingLine">
          <div className="fieldHeader" onClick={() => updateSort("sttName")}>
            STT
          </div>
          <div className="fieldHeader" onClick={() => updateSort("totalScore")}>
            Total Score
          </div>
          <div className="fieldHeader" onClick={() => updateSort("accuracy")}>
            Accuracy
          </div>
          <div className="fieldHeader" onClick={() => updateSort("speed")}>
            Speed
          </div>
          <div className="fieldHeader" onClick={() => updateSort("wer")}>
            WER
          </div>
          <div className="fieldHeader" onClick={() => updateSort("mer")}>
            MER
          </div>
          <div className="fieldHeader" onClick={() => updateSort("wil")}>
            WIL
          </div>
          <div className="fieldHeader" onClick={() => updateSort("sim")}>
            Similarity
          </div>
          <div className="fieldHeader" onClick={() => updateSort("dist")}>
            Lev-Distance
          </div>
        </div>
        {rankingList.sort(compare).map((ranking, i) => (
            <RankingLine
            key={i}
            stt={ranking.sttName}
            score={ranking.totalScore}
            accuracy={ranking.accuracy}
            speed={ranking.speed}
            wer={ranking.wer}
            mer={ranking.mer}
            wil={ranking.wil}
            sim={ranking.sim}
            dist={ranking.dist}
          />
        ))}
      </div>
    </div>
  );
}

export default Ranking;
