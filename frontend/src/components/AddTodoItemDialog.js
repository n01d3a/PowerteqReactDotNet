import React, { useState } from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, TextField, Typography } from '@material-ui/core';
import { addTodoItem } from '../services/todoServices';
import { getErrorMessageFromAxiosError } from '../utilities/axiosUtilities';
import { Alert } from '@material-ui/lab';

export default function AddTodoItemDialog(props) {
    const [errorMessage, setErrorMessage] = useState("");
    const [todoItemTitle, setTodoItemTitle] = useState("");
    const [todoItemDescription, setTodoItemDescription] = useState("");
    const [loading, setLoading] = useState(false);

    const handleTodoItemTitleChanged = (value) => {
        setTodoItemTitle(value);
    }

    const handleTodoItemDescriptionChanged = (value) => {
        setTodoItemDescription(value);
    }

    const clearAndCloseAddTodoItemDialog = () => {
        setErrorMessage("");
        setTodoItemTitle("");
        setTodoItemDescription("");
        props.onCloseRequested();
    }

    const handleAddTodoItemClick = async () => {
        setErrorMessage("");
        setLoading(true);

        try {
            const response = await addTodoItem({
                title: todoItemTitle,
                description: todoItemDescription
            });

            props.onAddedTodoItem(response.data);
            clearAndCloseAddTodoItemDialog();
        } catch (error) {
            setErrorMessage(getErrorMessageFromAxiosError(error));
        }

        setLoading(false);
    }

    return (
        <Dialog open={props.open}>
            <DialogTitle disableTypography><Typography variant="h6" align="center">Add Todo</Typography></DialogTitle>
            <DialogContent>
                {errorMessage &&
                    <Alert severity="error">{errorMessage}</Alert>
                }
                <form autoComplete="off">
                    <Grid container>
                        <TextField value={todoItemTitle} onChange={event => handleTodoItemTitleChanged(event.target.value)} required autoFocus margin="dense" label="Title" />
                    </Grid>
                    <Grid container>
                        <TextField value={todoItemDescription} onChange={event => handleTodoItemDescriptionChanged(event.target.value)} margin="dense" label="Description" variant="outlined" rows={4} rowsMax={10} multiline/>
                    </Grid>
                </form>

            </DialogContent>
            <DialogActions>
                <Button onClick={clearAndCloseAddTodoItemDialog} disabled={loading}>Cancel</Button>
                <Button onClick={handleAddTodoItemClick} disabled={loading}>Add</Button>
            </DialogActions>
        </Dialog>
    );
}