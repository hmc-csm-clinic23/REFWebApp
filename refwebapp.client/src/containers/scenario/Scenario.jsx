import { React, useState, useEffect } from "react";
import { AddScenarioCheckbox } from "../.././components";
import "./scenario.css";

function Scenario() {
  const [audioList, setAudioList] = useState([]);
  const [search, setSearch] = useState("");
  const [scenarioName, setScenarioName] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const audioSubmit = audioList.filter((audio) => audio.checked === true).map(({checked, ...scenario}) => scenario);

  async function populateAudioData(signal) {
    try {
      const response = await fetch('audiolist', { signal: signal });
      const data = await response.json();
      setAudioList(data.map((audio) => ({
        ...audio,
        checked: false,
      })));
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
    // fetch call to API goes here, where you then get access to `objects`
    // then set your toggles state
    populateAudioData();
  }, []);

  const updateAudioToggle = (i) => {
    setAudioList((audioList) =>
      audioList.with(i, {
        ...audioList[i],
        checked: !audioList[i].checked,
      }),
    );
  };

  const updateScenarios = async () => {
    console.log(scenarioName);
    console.log(audioSubmit);
    if ((audioSubmit.length != 0) && (scenarioName != "")){
        await fetch('addscenario',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Name: scenarioName,
                    Audios: audioSubmit,
                })
            });
        setAudioList(
          audioList.map((audio) => ({
            ...audio,
            checked: false,
          })),
        );
        setScenarioName("");
    }
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
        <li className="addScenarioItemHeader">
          <div className="leftSide">
            <span className="checkboxHidden">
            </span>
          </div>
          <div className="addScenarioContainer">
            <span className="itemText">Path</span>
            <span className="itemText">Ground Truth</span>
            <span className="itemText">Audio</span>
          </div>
        </li>
        {audioList
          .sort((a, b) => (a.path > b.path ? 1 : b.path > a.path ? -1 : 0))
          .map((item, i) => ({ item, i }))
          .filter(({ item }) =>
            search.toLowerCase() === ""
              ? item
              : item.path.toLowerCase().includes(search.toLowerCase()),
          )
          .map(({ item, i }) => {
          return (
          <AddScenarioCheckbox
              key={item.path}
              toggle={item.checked}
              setToggle={() => updateAudioToggle(i)}
              name={item.path}
              groundTruth={item.groundTruth}
              audioFile={item.path}
          />
          );
          })
        }
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
