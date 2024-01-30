import { Checkbox, DropdownHeader } from "../";

function SttDropdown({ toggle, setToggle, search, setSearch, list, setList }) {
  const updateSttToggle = (i) => {
    setList((list) =>
      list.with(i, {
        ...list[i],
        checked: !list[i].checked,
      }),
    );
  };

  return (
    <>
      <div className="selectButton">
        <DropdownHeader
          toggle={toggle}
          setToggle={setToggle}
          text={toggle ? "Search STTs" : "Select STTs"}
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
              <Checkbox
                key={stt.name}
                toggle={stt.checked}
                setToggle={() => updateSttToggle(i)}
                name={stt.name}
              />
            ))}
        </ul>
      )}
    </>
  );
}

export default SttDropdown;
