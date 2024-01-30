import { DropdownHeader } from "../";

function HistoryDropdown({
  name,
  toggle,
  setToggle,
  search,
  setSearch,
  list,
  setClickToggle,
}) {
  const updateSttToggle = (i) => {
    setToggle((toggle) => !toggle);
    setClickToggle(() => list[i].name);
  };

  return (
    <>
      <div className="selectButton">
        <DropdownHeader
          toggle={toggle}
          setToggle={setToggle}
          text={toggle ? "Search STTs" : name}
          search={search}
          setSearch={setSearch}
        />
      </div>
      {toggle && (
        <ul className="sttItems">
          {list
            .sort((a, b) => (a.name > b.name ? 1 : b.name > a.name ? -1 : 0))
            .map((stt, i) => ({ stt, i }))
            .filter(({ stt }) =>
              search.toLowerCase() === ""
                ? stt
                : stt.name.toLowerCase().includes(search.toLowerCase()),
            )
            .map(({ stt, i }) => (
              <>
                <li className="dropdownItem" onClick={() => updateSttToggle(i)}>
                  <span className="itemText">{stt.name}</span>
                </li>
              </>
            ))}
        </ul>
      )}
    </>
  );
}

export default HistoryDropdown;
