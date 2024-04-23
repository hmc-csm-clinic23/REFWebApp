import { React } from "react";
import { AiOutlineCheck } from "react-icons/ai";

function AddScenarioCheckbox({ toggle, setToggle, name, groundTruth, audioFile,  }) {
  return toggle ? (
    <li className="addScenarioItem" onClick={setToggle}>
      <div className="leftSide">
        <span className="checkbox">
          <AiOutlineCheck />
        </span>
      </div>
      <div className="addScenarioContainer">
        <span className="itemText">{name}</span>
        <span className="itemText">{groundTruth}</span>
        <audio controls src={audioFile} className=""></audio>
      </div>
    </li>
  ) : (
    <li className="addScenarioItem" onClick={setToggle}>
      <div className="leftSide">
        <span className="checkbox" />
      </div>
      <div className="addScenarioContainer">
        <span className="itemText">{name}</span>
        <span className="itemText">{groundTruth}</span>
        <audio controls src={audioFile} className=""></audio>
      </div>
    </li>
  );
}

export default AddScenarioCheckbox;
