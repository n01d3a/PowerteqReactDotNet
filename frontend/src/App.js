import React, { useState, useEffect } from 'react';
import { Card, CardHeader, Container, Fab, Typography } from '@material-ui/core';
import { Add as AddIcon } from '@material-ui/icons';
import { Alert } from '@material-ui/lab';
import { getAllTodoItems, deleteTodoItem } from './services/todoServices';
import { getErrorMessageFromAxiosError } from './utilities/axiosUtilities';
import TodoItemAccordion from './components/TodoItemAccordion';
import AddTodoItemDialog from './components/AddTodoItemDialog';
import EditTodoItemDialog from './components/EditTodoItemDialog';
import lodash from 'lodash';
import moment from 'moment';

export default function App() {
  const [errorMessage, setErrorMessage] = useState("");
  const [todoItems, setTodoItems] = useState([]);
  const [displayAddTodoItem, setDisplayAddTodoItem] = useState(false);
  const [displayEditTodoItem, setDisplayEditTodoItem] = useState(false);
  const [todoItemToUpdate, setTodoItemToUpdate] = useState(null);

  useEffect(() => {
    async function getInitialData() {
      await refreshData();
    }

    getInitialData();
  }, []);

  const refreshData = async () => {
    try {
      const response = await getAllTodoItems();
      let todoItems = response.data;
      lodash.forEach(todoItems, item => {
        item.createdDate = moment.utc(item.createdDate).local();
      });

      todoItems = lodash.sortBy(todoItems, ["createdDate"]);

      setTodoItems(todoItems);
    } catch (error) {
      setErrorMessage(getErrorMessageFromAxiosError(error));
    }
  }

  const handleAddTodoItemClick = () => {
    setDisplayAddTodoItem(true);
  }

  const handleOnCloseAddTodoItemDialogRequested = () => {
    setDisplayAddTodoItem(false);
  }

  const handleOnAddedTodoItem = async () => {
    await refreshData();
  }

  const handleEditTodoItemClick = async (todoItem) => {
    setTodoItemToUpdate(todoItem);
    setDisplayEditTodoItem(true);
  }

  const handleOnUpdatedTodoItem = async () => {
    await refreshData();
  }

  const handleOnCloseUpdateTodoItemDialogRequested = () => {
    setDisplayEditTodoItem(false);
  }

  const handleDeleteTodoItemClick = async (todoItem) => {
    try {
      await deleteTodoItem(todoItem.id);
      await refreshData();
    } catch (error) {
      setErrorMessage(getErrorMessageFromAxiosError(error));
    }
  }

  return (
    <React.Fragment>
      <header>
        <Container>
          <Typography variant="h2" align="center">Todo Manager</Typography>
        </Container>
        <hr />
      </header>
      {errorMessage &&
        <Alert severity="error">{errorMessage}</Alert>
      }
      {!errorMessage &&
        <React.Fragment>
          <Container>
            <Card>
              <CardHeader title="Todo Items" titleTypographyProps={{ align: "center", variant: "h4" }} action={<Fab onClick={handleAddTodoItemClick} size="medium" color="primary"><AddIcon /></Fab>}></CardHeader>
            </Card>
            <br />
            {
              todoItems.map(todoItem => <TodoItemAccordion todoItem={todoItem} key={todoItem.id} onEditClicked={handleEditTodoItemClick} onDeleteClicked={handleDeleteTodoItemClick} />)
            }
          </Container>

          <AddTodoItemDialog open={displayAddTodoItem} onAddedTodoItem={handleOnAddedTodoItem} onCloseRequested={handleOnCloseAddTodoItemDialogRequested} />
          <EditTodoItemDialog open={displayEditTodoItem} todoItem={todoItemToUpdate} onUpdatedTodoItem={handleOnUpdatedTodoItem} onCloseRequested={handleOnCloseUpdateTodoItemDialogRequested} />
        </React.Fragment >
      }
    </React.Fragment >
  );
}