import { AiOutlineCheck } from "react-icons/ai";

function Checkbox({ key, toggle, setToggle, name }) {
  return toggle ? (
    <li className="dropdownItem" onClick={setToggle}>
      <span className="checkbox">
        <AiOutlineCheck />
      </span>
      <span className="itemText">{name}</span>
    </li>
  ) : (
    <li className="dropdownItem" onClick={setToggle}>
      <span className="checkbox" />
      <span className="itemText">{name}</span>
    </li>
  );
}

export default Checkbox;
