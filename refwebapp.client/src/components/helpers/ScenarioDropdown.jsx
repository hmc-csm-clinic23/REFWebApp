import { Checkbox, WeightSlider, DropdownHeader } from "../";

function ScenarioDropdown({
  toggle,
  setToggle,
  search,
  setSearch,
  list,
  setList,
}) {
  const updateScenarioToggle = (i) => {
    setList((list) =>
      list.with(i, {
        ...list[i],
        checked: !list[i].checked,
        weight: list[i].checked ? +0 : +50,
      }),
    );
  };

  const updateScenarioWeight = (i, val) => {
    setList((list) =>
      list.with(i, {
        ...list[i],
        weight: +val,
      }),
    );
  };

  return (
    <>
      <div className="selectButton">
        <DropdownHeader
          toggle={toggle}
          setToggle={setToggle}
          text={toggle ? "Search Scenarios" : "Select Scenarios"}
          search={search}
          setSearch={setSearch}
        />
      </div>
      {toggle && (
        <ul className="scenarioItems">
          {list
            .sort((a, b) => (a.name > b.name ? 1 : b.name > a.name ? -1 : 0))
            .map((scenario, i) => ({ scenario, i }))
            .filter(({ scenario }) =>
              search.toLowerCase() === ""
                ? scenario
                : scenario.name.toLowerCase().includes(search.toLowerCase()),
            )
            .map(({ scenario, i }) => (
              <>
                <Checkbox
                  key={scenario.name}
                  toggle={scenario.checked}
                  setToggle={() => updateScenarioToggle(i)}
                  name={scenario.name}
                />
                <WeightSlider
                  toggle={scenario.checked}
                  weight={scenario.weight}
                  setWeight={updateScenarioWeight}
                  i={i}
                  scenarioSum={list
                    .map((scenario) => scenario.weight)
                    .reduce((a, b) => a + b)}
                />
              </>
            ))}
        </ul>
      )}
    </>
  );
}

export default ScenarioDropdown;
