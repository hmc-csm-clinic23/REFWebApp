import { React, useState, useEffect } from "react";
import { AddScenarioCheckbox } from "../.././components";
import "./scenario.css";

function Scenario() {
  const [audioList, setAudioList] = useState([]);
  const [search, setSearch] = useState("");
  const [scenarioName, setScenarioName] = useState("");
  const [newScenario, setNewScenario] = useState([]);
  const audioSubmit = audioList.filter((audio) => audio.checked === true);

  const audios = [
    {
      name: "space station test 1",
      audioFile: "spacestationtest1.wav",
    },
    {
      name: "space station test 2",
      audioFile: "spacestationtest2.wav",
    },
    {
      name: "mars test 5",
      audioFile: "marstest5.wav",
    },
    {
      name: "johnson space center test 8",
      audioFile: "johnsonspacecentertest8.wav",
    },
    {
      name: "mars test 7",
      audioFile: "marstest7.wav",
    },
    {
      name: "johnson space center test 6",
      audioFile: "johnsonspacecentertest6.wav",
    },
    {
      name: "neutral buoyancy lab test 12",
      audioFile: "neutralbuoyancylabtest12.wav",
    },
  ];

  useEffect(() => {
    // fetch call to API goes here, where you then get access to `objects`
    // then set your toggles state
    setAudioList(
      audios.map((audio, i) => ({
        ...audio,
        checked: false,
      })),
    );
  }, []);

  const updateAudioToggle = (i) => {
    setAudioList((audioList) =>
      audioList.with(i, {
        ...audioList[i],
        checked: !audioList[i].checked,
      }),
    );
  };

  const updateScenarios = () => {
    console.log(scenarioName);
    console.log(audioSubmit);
    setNewScenario((newScenario) => ({
      ...newScenario,
      name: scenarioName,
      audio: audioSubmit,
    }));
    setAudioList(
      audioList.map((audio) => ({
        ...audio,
        checked: false,
      })),
    );
    setScenarioName("");
    // fetch using setNewScenario here
  };

  return (
    <div className="scenarioPage">
      <input
        type="text"
        className="searchAudio"
        placeholder="Scenario Name"
        value={scenarioName}
        onChange={(e) => setScenarioName(e.target.value)}
      />
      <input
        type="text"
        className="searchAudio"
        placeholder="Search audio files"
        onChange={(e) => setSearch(e.target.value)}
      />
      <ul className="audioItems">
        {audioList
          .sort((a, b) => (a.name > b.name ? 1 : b.name > a.name ? -1 : 0))
          .map((item, i) => ({ item, i }))
          .filter(({ item }) =>
            search.toLowerCase() === ""
              ? item
              : item.name.toLowerCase().includes(search.toLowerCase()),
          )
          .map(({ item, i }) => (
            <>
              <AddScenarioCheckbox
                key={item.name}
                toggle={item.checked}
                setToggle={() => updateAudioToggle(i)}
                name={item.name}
                audioFile={item.audioFile}
              />
            </>
          ))}
      </ul>
      <div className="submitButton" onClick={() => updateScenarios()}>
        <div className="submit">
          <span className="submitText">Submit</span>
        </div>
      </div>
    </div>
  );
}

export default Scenario;
