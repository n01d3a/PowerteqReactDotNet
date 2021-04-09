import React, { useEffect, useState } from 'react';
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, TextField, Typography } from '@material-ui/core';
import { updateTodoItem } from '../services/todoServices';
import { getErrorMessageFromAxiosError } from '../utilities/axiosUtilities';
import { Alert } from '@material-ui/lab';

export default function EditTodoItemDialog(props) {
    const [errorMessage, setErrorMessage] = useState("");
    const [todoItemTitle, setTodoItemTitle] = useState("");
    const [todoItemDescription, setTodoItemDescription] = useState("");
    const [loading, setLoading] = useState(false);
    useEffect(() => {
        setTodoItemTitle(props.todoItem?.title ?? "");
        setTodoItemDescription(props.todoItem?.description ?? "");
    }, [props.todoItem]);

    const handleTodoItemTitleChanged = (value) => {
        setTodoItemTitle(value);
    }

    const handleTodoItemDescriptionChanged = (value) => {
        setTodoItemDescription(value);
    }

    const clearAndCloseUpdateTodoItemDialog = () => {
        setErrorMessage("");
        setTodoItemTitle(props.todoItem?.title ?? "");
        setTodoItemDescription(props.todoItem?.description ?? "");
        props.onCloseRequested();
    }

    const handleUpdateTodoItemClick = async () => {
        setErrorMessage("");
        setLoading(true);

        try {
            const response = await updateTodoItem({
                id: props.todoItem.id,
                title: todoItemTitle ?? props.todoItem?.title,
                description: todoItemDescription ?? props.todoItem?.description
            });

            props.onUpdatedTodoItem(response.data);
            clearAndCloseUpdateTodoItemDialog();
        } catch (error) {
            setErrorMessage(getErrorMessageFromAxiosError(error));
        }

        setLoading(false);
    }

    return (
        <Dialog open={props.open}>
            <DialogTitle disableTypography><Typography variant="h6" align="center">Update Todo</Typography></DialogTitle>
            <DialogContent>
                {errorMessage &&
                    <Alert severity="error">{errorMessage}</Alert>
                }
                <form autoComplete="off">
                    <Grid>
                        <TextField value={todoItemTitle} onChange={event => handleTodoItemTitleChanged(event.target.value)} required margin="dense" label="Title" />
                    </Grid>
                    <Grid>
                        <TextField value={todoItemDescription} onChange={event => handleTodoItemDescriptionChanged(event.target.value)} margin="dense" label="Description" variant="outlined" rows={4} rowsMax={10} multiline/>
                    </Grid>
                </form>
            </DialogContent>
            <DialogActions>
                <Button onClick={clearAndCloseUpdateTodoItemDialog} disabled={loading}>Cancel</Button>
                <Button onClick={handleUpdateTodoItemClick} disabled={loading}>Update</Button>
            </DialogActions>
        </Dialog>
    );
}