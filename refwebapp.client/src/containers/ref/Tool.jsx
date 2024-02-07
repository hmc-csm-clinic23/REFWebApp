import { React, useState, useEffect, useContext } from "react";
import { Context } from "../../App";
import { NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import { Checkbox, SttDropdown, ScenarioDropdown } from "../.././components";
import "./tool.css";

function Tool() {
  const { setSelections } = useContext(Context);
  const [sttToggle, setSttToggle] = useState(false);
  const [scenarioToggle, setScenarioToggle] = useState(false);
  const [useStoreToggle, setUseStoreToggle] = useState(false);
  const [updateStoreToggle, setUpdateStoreToggle] = useState(false);
  const [sttSearch, setSttSearch] = useState("");
  const [scenarioSearch, setScenarioSearch] = useState("");
  const [sttList, setSttList] = useState([]);
  const [scenarioList, setScenarioList] = useState([]);
  const sttSubmit = sttList.filter((stt) => stt.checked === true);
  const scenarioSubmit = scenarioList.filter(
    (scenario) => scenario.checked === true,
  );

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
    populateSttData();
    populateScenarioData();
  }, []);

  const updateSelections = () => {
    setSelections((prevState) => ({
      ...prevState,
      stts: sttSubmit,
      scenarios: scenarioSubmit,
      useStore: useStoreToggle,
      updateStore: updateStoreToggle,
    }));
  };

  return (
    <div className="toolPage">
      <div className="buttonContainer">
        <SttDropdown
          toggle={sttToggle}
          setToggle={setSttToggle}
          search={sttSearch}
          setSearch={setSttSearch}
          list={sttList}
          setList={setSttList}
        />
      </div>
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
      <div className="buttonContainer">
        <div className="storeOption">
          <Checkbox
            key={"useStore"}
            toggle={useStoreToggle}
            setToggle={() => setUseStoreToggle(!useStoreToggle)}
            name={"Use Stored Metrics"}
          />
        </div>
      </div>
      <div className="buttonContainer">
        <div className="storeOption">
          <Checkbox
            key={"updateStore"}
            toggle={updateStoreToggle}
            setToggle={() => setUpdateStoreToggle(!updateStoreToggle)}
            name={"Update Stored Metrics"}
          />
        </div>
      </div>
      <div className="submitButton" onClick={() => updateSelections()}>
        <NavLink tag={Link} to="/evaluation">
          <div className="submit">
            <span className="submitText">Submit</span>
          </div>
        </NavLink>
      </div>
    </div>
  );
}

export default Tool;
