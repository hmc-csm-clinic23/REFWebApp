import { React, useState, createContext } from "react";
import { Routes, Route } from "react-router-dom";
import {
  Home,
  Tool,
  Evaluation,
  Ranking,
  History,
  Scenario,
  Guide,
} from "./containers";
import { NavMenu } from "./components";
import "./app.css";

export const Context = createContext();

function App() {
  const [selections, setSelections] = useState([]);
  return (
    <div className="App">
      <div>
        <NavMenu />
        <Routes>
          <Route path="/" element={<Home />} />
        </Routes>
        <Context.Provider value={{ selections, setSelections }}>
          <Routes>
            <Route path="/tool" element={<Tool />} />
          </Routes>
          <Routes>
            <Route path="/evaluation" element={<Evaluation />} />
          </Routes>
        </Context.Provider>
        <Routes>
          <Route path="/ranking" element={<Ranking />} />
        </Routes>
        <Routes>
          <Route path="/history" element={<History />} />
        </Routes>
        <Routes>
          <Route path="/scenario" element={<Scenario />} />
        </Routes>
        <Routes>
          <Route path="/guide" element={<Guide />} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
