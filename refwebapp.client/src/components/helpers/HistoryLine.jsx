import { React, useState, useEffect } from "react";
import { AiOutlineMinus, AiOutlinePlus } from "react-icons/ai";
import { AudioList } from "../.././components";

function HistoryLine({
  scenario,
  score,
  accuracy,
  speed,
  wer,
  mer,
  wil,
  sim,
  dist,
  list,
  closeToggle,
  setCloseToggle,
}) {
  const [toggle, setToggle] = useState(false);
  const [clickToggle, setClickToggle] = useState(null);
  const [audioList, setAudioList] = useState([]);

  const updateSttToggle = (i) => {
    setToggle((toggle) => !toggle);
    setClickToggle(() => list[i].name);
  };

  const audios = [
    {
      name: "space station test 1",
      groundTruth: "hi how are you today",
      transcription: "hi how are you tay",
      wer: 84,
    },
    {
      name: "space station test 2",
      groundTruth: "hi how are you today",
      transcription: "hi how are you tay",
      wer: 93,
    },
    {
      name: "mars test 5",
      groundTruth: "woah what is up with you",
      transcription: "woah what's up with you",
      wer: 92,
    },
    {
      name: "johnson space center 8",
      groundTruth: "it is such a nice day",
      transcription: "it's such a rice day",
      wer: 81,
    },
    {
      name: "mars test 7",
      groundTruth: "I can't wait to go outside",
      transcription: "I can't weight to go outside",
      wer: 89,
    },
    {
      name: "johnson space center 6",
      groundTruth: "I can't believe how hot it is today",
      transcription: "I can't believe how not it's today",
      wer: 87,
    },
    {
      name: "neutral buoyancy lab test 12",
      groundTruth: "I don't want to do a dive till next week",
      transcription: "I don't want to do a dive teal next week",
      wer: 92,
    },
  ];

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    if (clickToggle !== null) {
      setAudioList(
        audios.map((audio) => ({
          ...audio,
        })),
      );
    }
  }, [clickToggle]);

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `stts`
    // then set your sttList state
    if (closeToggle) {
      setClickToggle(() => null);
      setAudioList(() => []);
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
              {list
                .sort((a, b) =>
                  a.name > b.name ? 1 : b.name > a.name ? -1 : 0,
                )
                .map((evaluation, i) => (
                  <>
                    <li
                      className="dropdownItem"
                      onClick={() => updateSttToggle(i)}
                    >
                      <span className="itemText">{evaluation.name}</span>
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
              setAudioList(() => []);
              setClickToggle(() => false);
            }}
          >
            X
          </button>
          <ul className="historyItems">
            <li className="audioList">
              <div className="evalsText">Name</div>
              <div className="evalsText">Ground Truth</div>
              <div className="evalsText">Transcription</div>
              <div className="evalsText">WER</div>
              <div className="evalsText">Audio File</div>
            </li>
            <div className="evalsText" />
            {audioList
              .sort((a, b) => (a.name > b.name ? 1 : b.name > a.name ? -1 : 0))
              .map((audio) => (
                <>
                  <AudioList
                    key={audio.name}
                    name={audio.name}
                    groundTruth={audio.groundTruth}
                    transcription={audio.transcription}
                    wer={audio.wer}
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
