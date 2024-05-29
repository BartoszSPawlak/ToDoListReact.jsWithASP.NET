import "./App.css";
import { useState } from "react";
import { useEffect } from "react";
import { Form } from "./components/Form";
import { Item } from "./components/Item";

function App() {
  var [add, setAdd] = useState(false);
  const [items, setItems] = useState([]);

  useEffect(() => {
    fetch("https://localhost:7156/ToDo", {
      method: "GET",
    }).then((response) => {
      if (response.ok) {
        response.json().then((data) => {
          setItems(data);
        });
      }
    });
  }, []);

  function setItem(newTodoName) {
    setAdd(false);

    const newItem = {
      Name: newTodoName,
      IsDone: false,
    };

    fetch("https://localhost:7156/ToDo", {
      method: "POST",
      body: JSON.stringify(newItem),
      headers: {
        "Content-Type": "application/json",
      },
    }).then((response) => {
      if (response.ok) {
        response.json().then((data) => {
          setItems((prevTodo) => {
            return [...prevTodo, data];
          });
        });
      }
    });
  }

  function finishItem(id) {
    const idAndIsDoneOfItemToPatch = {
      IsDone: true,
      Id: id,
    };

    fetch(`https://localhost:7156/ToDo/`, {
      method: "PATCH",
      body: JSON.stringify(idAndIsDoneOfItemToPatch),
      headers: { "Content-Type": "application/json" },
    }).then((response) => {
      if (response.ok) {
        response.json().then((data) => {
          setItems(data);
        });
      }
    });
  }

  function deleteItem(id) {
    fetch(`https://localhost:7156/ToDo/`, {
      method: "DELETE",
      body: JSON.stringify(id),
      headers: { "Content-Type": "application/json" },
    }).then((response) => {
      if (response.ok) {
        response.json().then((data) => {
          setItems(data);
        });
      }
    });
  }

  return (
    <>
      <div className="min-h-screen bg-blue">
        <div className="min-w-[400px] max-h-[calc(100vh-60px)] rounded-xl bg-white m-auto absolute left-1/2 ml-[-192px] mt-[30px] mb-[30px] px-5 pb-[24px]">
          <header className="flex justify-between font-bold py-3 ">
            <div>
              <h1 className="text-[28px] box-border m-0 p-0">Do zrobienia</h1>
              <h2 className="text-[25px] box-border m-0 p-0">
                {items.length} zadań
              </h2>
            </div>
            {add ? (
              ""
            ) : (
              <button
                className="bg-blue rounded-3xl border border-blue pb-[6px] w-11 h-11 text-white text-center text-2xl cursor-pointer mt-5 font-normal hover:text-blue hover:bg-white hover:border-blue"
                onClick={() => {
                  setAdd((prevState) => {
                    return { ...prevState, add: !add };
                  });
                }}
              >
                +
              </button>
            )}
          </header>
          {add ? (
            <Form onFormSubmit={(newTodoName) => setItem(newTodoName)} />
          ) : (
            ""
          )}

          <ul className="max-h-[calc(80vh)] overflow-auto">
            {items.map(({ id, name, isDone }) => (
              <Item
                key={id}
                name={name}
                done={isDone}
                finishItem={() => finishItem(id)}
                deleteItem={() => deleteItem(id)}
              />
            ))}
          </ul>
        </div>
      </div>
    </>
  );
}

export default App;
