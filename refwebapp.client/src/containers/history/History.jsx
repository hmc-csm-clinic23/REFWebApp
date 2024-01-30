import { React, useState, useEffect } from "react";
import { HistoryLine, HistoryDropdown } from "../.././components";
import "./history.css";

function History() {
  const [sttToggle, setSttToggle] = useState(false);
  const [sttSearch, setSttSearch] = useState("");
  const [sttList, setSttList] = useState([]);
  const [rankingList, setRankingList] = useState([]);
  const [sort, setSort] = useState("score");
  const [sortToggle, setSortToggle] = useState(1);
  const [clickToggle, setClickToggle] = useState(null);
  const [closeToggle, setCloseToggle] = useState(false);

  const stts = [
    {
      name: "OpenAI Whisper",
    },
    {
      name: "Google Chirp",
    },
    {
      name: "Meta MMS",
    },
    {
      name: "DeepGram",
    },
    {
      name: "PaddleSpeech",
    },
    {
      name: "Amazon Transcribe",
    },
    {
      name: "Microsoft Azure",
    },
  ];

  const scenarioEval1 = [
    {
      name: "Eval 1",
    },
    {
      name: "Eval 2",
    },
    {
      name: "Eval 3",
    },
    {
      name: "Eval 4",
    },
    {
      name: "Eval 5",
    },
    {
      name: "Eval 6",
    },
    {
      name: "Eval 7",
    },
  ];

  const scenarioEval2 = [
    {
      name: "Eval 1",
    },
    {
      name: "Eval 2",
    },
    {
      name: "Eval 3",
    },
  ];

  const scenarioEval3 = [
    {
      name: "Eval 1",
    },
    {
      name: "Eval 2",
    },
    {
      name: "Eval 3",
    },
    {
      name: "Eval 4",
    },
    {
      name: "Eval 5",
    },
  ];

  const rankings = [
    {
      scenario: "Loud",
      score: 92,
      accuracy: 97,
      speed: 2134,
      memory: 152,
      usability: "terrific",
      api: "yes",
      eval: scenarioEval1,
    },
    {
      scenario: "Quiet",
      score: 80,
      accuracy: 85,
      speed: 1581,
      memory: 341,
      usability: "great",
      api: "yes",
      eval: scenarioEval2,
    },
    {
      scenario: "Noisy",
      score: 78,
      accuracy: 87,
      speed: 1881,
      memory: 293,
      usability: "bad",
      api: "no",
      eval: scenarioEval1,
    },
    {
      scenario: "Sparse",
      score: 93,
      accuracy: 98,
      speed: 2023,
      memory: 137,
      usability: "terrific",
      api: "no",
      eval: scenarioEval1,
    },
    {
      scenario: "Windy",
      score: 64,
      accuracy: 74,
      speed: 1032,
      memory: 562,
      usability: "good",
      api: "yes",
      eval: scenarioEval3,
    },
  ];

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    setSttList(
      stts.map((stt) => ({
        ...stt,
        checked: false,
      })),
    );
    setRankingList(
      rankings.map((ranking) => ({
        ...ranking,
      })),
    );
  }, []);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    if (clickToggle !== null) {
      console.log(clickToggle);
    }
  }, [clickToggle]);

  const updateSort = (field) => {
    setSort(field);
    setSortToggle(sort === field ? -sortToggle : 1);
    setCloseToggle(true);
  };

  function compare(a, b) {
    const bool = sort === "scenario" || sort === "speed" || sort === "memory";
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
        <HistoryDropdown
          name={clickToggle ? `${clickToggle}` : "Select STTs"}
          toggle={sttToggle}
          setToggle={setSttToggle}
          search={sttSearch}
          setSearch={setSttSearch}
          list={sttList}
          setClickToggle={setClickToggle}
        />
      </div>
      <div className="rankingContainer">
        <div className="historyLine">
          <div className="fieldHeader" onClick={() => updateSort("scenario")}>
            Scenario
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
          <div className="fieldHeader" onClick={() => setCloseToggle(true)}>
            History
          </div>
        </div>
        {rankingList.sort(compare).map((ranking) => (
          <HistoryLine
            scenario={ranking.scenario}
            score={ranking.score}
            accuracy={ranking.accuracy}
            speed={ranking.speed + "ms"}
            memory={ranking.memory + "mb"}
            list={ranking.eval}
            closeToggle={closeToggle}
            setCloseToggle={setCloseToggle}
          />
        ))}
      </div>
    </div>
  );
}

export default History;
