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

  const scenarios = [
    {
      name: "Loud",
    },
    {
      name: "Quiet",
    },
    {
      name: "Noisy",
    },
    {
      name: "Sparse",
    },
    {
      name: "Windy",
    },
    {
      name: "Space",
    },
    {
      name: "Clear",
    },
  ];

  const rankings = [
    {
      stt: "Whisper",
      score: 92,
      accuracy: 97,
      speed: 2134,
      memory: 152,
      usability: "terrific",
      api: "yes",
    },
    {
      stt: "Chirp",
      score: 80,
      accuracy: 85,
      speed: 1581,
      memory: 341,
      usability: "great",
      api: "yes",
    },
    {
      stt: "MMS",
      score: 78,
      accuracy: 87,
      speed: 1881,
      memory: 293,
      usability: "bad",
      api: "no",
    },
    {
      stt: "Deepgram",
      score: 93,
      accuracy: 98,
      speed: 2023,
      memory: 137,
      usability: "terrific",
      api: "no",
    },
    {
      stt: "Azure",
      score: 64,
      accuracy: 74,
      speed: 1032,
      memory: 562,
      usability: "good",
      api: "yes",
    },
  ];
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

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `scenarios`
    // then set your scenarioList state
    populateScenarioData();
    setRankingList(
      rankings.map((ranking) => ({
        ...ranking,
      })),
    );
  }, []);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `rankings`
    // set a fetch buffer so there isn't constant fetching while the checks and weightsliders are changing
    // then set your rankingList state
    setRankingList(
      rankings.map((ranking) => ({
        ...ranking,
      })),
    );
  }, [scenarioList]);

  const updateSort = (field) => {
    setSort(field);
    setSortToggle(sort === field ? -sortToggle : 1);
  };

  function compare(a, b) {
    const bool = sort === "stt" || sort === "speed" || sort === "memory";
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
          <div className="fieldHeader" onClick={() => updateSort("stt")}>
            STT
          </div>
          <div className="fieldHeader" onClick={() => updateSort("score")}>
            Total Score
          </div>
          <div className="fieldHeader" onClick={() => updateSort("accuracy")}>
            Accuracy
          </div>
          <div className="fieldHeader" onClick={() => updateSort("speed")}>
            Speed
          </div>
          <div className="fieldHeader" onClick={() => updateSort("memory")}>
            Memory
          </div>
          <div className="fieldHeader" onClick={() => updateSort("usability")}>
            Usability
          </div>
          <div className="fieldHeader" onClick={() => updateSort("api")}>
            API
          </div>
        </div>
        {rankingList.sort(compare).map((ranking, index) => (
          <RankingLine
            stt={ranking.stt}
            score={ranking.score}
            accuracy={ranking.accuracy}
            speed={ranking.speed + "ms"}
            memory={ranking.memory + "mb"}
            usability={ranking.usability}
            api={ranking.api}
          />
        ))}
      </div>
    </div>
  );
}

export default Ranking;
