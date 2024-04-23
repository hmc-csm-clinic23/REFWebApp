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
  const [loading, setLoading] = useState(false);

  async function populateSttData() {
    const response = await fetch('sttlist');
    const data = await response.json();
    setSttList(
      data.map((stt) => ({
        ...stt,
        checked: false,
      })),
    );
  }

  async function populateHistoryData() {
    const response = await fetch('histories',
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            SttName: clickToggle
        })
      });
    setLoading(true)
    const data = await response.json();
    setRankingList(data);
    setLoading(false);
  }

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    populateSttData();
  }, []);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `rankings`
    // set a fetch buffer so there isn't constant fetching while the checks and weightsliders are changing
    // then set your rankingList state
    if ((!loading) && (clickToggle)) {
        populateHistoryData();
    }
  }, [clickToggle]);

  const updateSort = (field) => {
    setSort(field);
    setSortToggle(sort === field ? -sortToggle : 1);
    setCloseToggle(true);
  };

  function compare(a, b) {
    const bool = sort === "scenarioName" || sort === "speed" || sort === "wer" || sort === "mer" || sort === "wil" || sort === "sim" || sort === "dist";
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
          <div className="fieldHeader" onClick={() => updateSort("scenarioName")}>
            Scenario
          </div>
          <div className="fieldHeader" onClick={() => updateSort("accuracy")}>
            Accuracy
          </div>
          <div className="fieldHeader" onClick={() => updateSort("speed")}>
            Speed
          </div>
          <div className="fieldHeader" onClick={() => updateSort("wer")}>
            Wer
          </div>
          <div className="fieldHeader" onClick={() => updateSort("mer")}>
            Mer
          </div>
          <div className="fieldHeader" onClick={() => updateSort("wil")}>
            Wil
          </div>
          <div className="fieldHeader" onClick={() => updateSort("sim")}>
            Sim
          </div>
          <div className="fieldHeader" onClick={() => updateSort("dist")}>
            L-Dist
          </div>
          <div className="fieldHeader" onClick={() => setCloseToggle(true)}>
            History
          </div>
        </div>
        {rankingList.sort(compare).map((ranking, i) => (
          <HistoryLine
            key={i}
            scenario={ranking.scenarioName}
            accuracy={ranking.accuracy}
            speed={ranking.speed}
            wer={ranking.wer}
            mer={ranking.mer}
            wil={ranking.wil}
            sim={ranking.sim}
            dist={ranking.dist}
            stt={clickToggle}
            closeToggle={closeToggle}
            setCloseToggle={setCloseToggle}
          />
        ))}
      </div>
    </div>
  );
}

export default History;
