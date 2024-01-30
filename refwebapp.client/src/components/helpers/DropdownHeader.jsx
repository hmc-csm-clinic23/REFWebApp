import { AiOutlineMinus, AiOutlinePlus } from "react-icons/ai";

function DropdownHeader({ toggle, setToggle, text, search, setSearch }) {
  return toggle ? (
    <>
      <input
        type="text"
        placeholder={text}
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
      <AiOutlineMinus
        className="plusMinus"
        size={27}
        onClick={() => setToggle(false)}
      />
    </>
  ) : (
    <>
      <div className="buttonText">{text}</div>
      <AiOutlinePlus
        className="plusMinus"
        size={27}
        onClick={() => setToggle(true)}
      />
    </>
  );
}

export default DropdownHeader;
