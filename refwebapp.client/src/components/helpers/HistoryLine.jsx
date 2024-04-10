import { React, useState, useEffect } from "react";
import { AiOutlineMinus, AiOutlinePlus } from "react-icons/ai";
import { AudioList } from "../.././components";

function HistoryLine({
  scenario,
  accuracy,
  speed,
  wer,
  mer,
  wil,
  sim,
  dist,
  stt,
  closeToggle,
  setCloseToggle,
}) {
  const [toggle, setToggle] = useState(false);
  const [clickToggle, setClickToggle] = useState(null);
  const [evalList, setEvalList] = useState([]);
  const [timestamp, setTimestamp] = useState();
  const [transcriptionList, setTranscriptionList] = useState([]);

  const updateSttToggle = (time, i) => {
    setToggle((toggle) => !toggle);
    setClickToggle(() => `Eval ${i+1}`);
    setTimestamp(time);
    setEvalList([])
  };
  async function populateEvalData() {
    const response = await fetch('evallist',
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            SttName: stt,
            ScenarioName: scenario
        })
      });
    const data = await response.json();
    setEvalList(data);
  }

  async function populateTranscriptionData() {
    const response = await fetch('evalhistories',
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            SttName: stt,
            ScenarioName: scenario,
            Timestamp: timestamp
        })
      });
    const data = await response.json();
    setTranscriptionList(data);
  }

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    setToggle(false)
    setClickToggle(null)
  }, [stt]);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    populateEvalData()
  }, [toggle]);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    if (timestamp !== null) {
      populateTranscriptionData()
    }
  }, [timestamp]);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    if (closeToggle) {
      setClickToggle(() => null);
      setTranscriptionList(() => []);
      setCloseToggle(() => false);
    }
  }, [closeToggle]);

  return (
    <>
      <div className="historyLine">
        <div className="rankingField">{scenario}</div>
        <div className="rankingField">{accuracy.toFixed(0)}</div>
        <div className="rankingField">{speed}</div>
        <div className="rankingField">{wer.toFixed(2)}</div>
        <div className="rankingField">{mer.toFixed(2)}</div>
        <div className="rankingField">{wil.toFixed(2)}</div>
        <div className="rankingField">{sim.toFixed(2)}</div>
        <div className="rankingField">{dist.toFixed(2)}</div>
        <div className="rankingField">
          {toggle ? (
            <>
              <span>
                {toggle
                  ? "Select Eval"
                  : clickToggle
                    ? `${clickToggle}`
                    : "Select Eval"}
              </span>
              <AiOutlineMinus
                onClick={() => setToggle(false)}
                className="historyPlusMinus"
              />
            </>
          ) : (
            <>
              <span>
                {toggle
                  ? "Select Eval"
                  : clickToggle
                    ? `${clickToggle}`
                    : "Select Eval"}
              </span>
              <AiOutlinePlus
                onClick={() => setToggle(true)}
                className="historyPlusMinus"
              />
            </>
          )}
          {toggle && (
            <ul className="sttItems">
              {evalList
                .map((time, i) => (
                  <>
                    <li
                      className="dropdownItem"
                      onClick={() => updateSttToggle(time, i)}
                    >
                      <span className="itemText">Eval {i+1}</span>
                    </li>
                  </>
                ))}
            </ul>
          )}
        </div>
      </div>
      {clickToggle && (
        <div className="historyWrapper">
          <button
            className="xButton"
            onClick={() => {
              setTranscriptionList(() => []);
              setClickToggle(() => false);
            }}
          >
            X
          </button>
          <ul className="historyItems">
            <li className="audioList">
              <div className="evalsText">Ground Truth</div>
              <div className="evalsText">Transcription</div>
              <div className="evalsText">Wer</div>
              <div className="evalsText">Mer</div>
              <div className="evalsText">Wil</div>
              <div className="evalsText">Sim</div>
              <div className="evalsText">L-Dist</div>
              <div className="evalsText">Audio File</div>
            </li>
            <div className="evalsText" />
            {transcriptionList
              .sort((a, b) => (a.name > b.name ? 1 : b.name > a.name ? -1 : 0))
              .map((audio) => (
                <>
                  <AudioList
                    groundTruth={audio.groundTruth}
                    transcription={audio.transcription}
                    wer={audio.wer}
                    mer={audio.mer}
                    wil={audio.wil}
                    sim={audio.sim}
                    dist={audio.dist}
                    audioFile={audio.audioFile}
                  />
                </>
              ))}
          </ul>
        </div>
      )}
    </>
  );
}

export default HistoryLine;
