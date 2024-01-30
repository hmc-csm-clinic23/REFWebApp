import { React } from "react";
import { AiOutlineCheck } from "react-icons/ai";

function AddScenarioCheckbox({ key, toggle, setToggle, name, audioFile }) {
  return toggle ? (
    <li className="addScenarioItem" onClick={setToggle}>
      <div className="leftSide">
        <span className="checkbox">
          <AiOutlineCheck />
        </span>
        <span className="itemText">{name}</span>
      </div>
      <audio controls src={audioFile} className=""></audio>
    </li>
  ) : (
    <li className="addScenarioItem" onClick={setToggle}>
      <div className="leftSide">
        <span className="checkbox" />
        <span className="itemText">{name}</span>
      </div>
      <audio controls src={audioFile} className=""></audio>
    </li>
  );
}

export default AddScenarioCheckbox;
