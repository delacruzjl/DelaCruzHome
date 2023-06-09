import React, { useState } from "react";
import { useForm, SubmitHandler } from "react-hook-form";

type Inputs = {
    fullName: string
    email: string
    content: string
}

export default function ContactApp() {
    const [submitted, setSubmitted] = useState(false);
    const [hasErrors, setHasErrors] = useState(false);

    const { register, handleSubmit, formState: { errors } } = useForm<Inputs>()
    const onSubmit: SubmitHandler<Inputs> = async (data) => {

        let formData = JSON.stringify(data)
        const response = await fetch("/api/Emailer", { method: "POST", body: formData})
    setSubmitted(response.ok)
    setHasErrors(!response.ok)

    const output = await response.text()
        console.debug(output)
    }

    return (
        <>
            <form onSubmit={handleSubmit(onSubmit)} className={hasErrors || submitted ? 'is-hidden' : '' }>
                <div className="field">
                    <label className="label" htmlFor="fullName">Your Name (Required)</label>
                    <input type="text" name="fullName" id="fullName" className="input" 
                    {...register("fullName", { required: true })}
                    />

                    {errors.fullName && <div className="is-danger" style={{display: 'block'}} >A name is required.</div>}

                </div>
                <div className="field">
                    <label className="label" htmlFor="email">Your Email Address (Required)</label>
                    <input type="email" name="email" id="email" className="input" 
                    {...register("email", { required: true })}
                    />

                    {errors.email && <div className="is-danger" style={{display: 'block'}} >An Email is required.</div>}

                </div>
                <div className="field">
                    <label className="label" htmlFor="content">Your Message Here</label>
                    <textarea name="content" id="content" className="input"
                    {...register("content", { required: true })}
                    ></textarea>

                    {errors.content && <div className="is-danger" style={{display: 'block'}} >A message is required.</div>}
                </div>
                <button type="submit" className="btn btn-primary">Send Now</button>
            </form>

            <article className={ hasErrors ? 'message is-danger is-visible' : 'is-hidden'  }>
                <div className="message-header">
                    <p></p>
                    <button className="delete" aria-label="delete"
                    onClick={() => setHasErrors(false) }></button>
                </div>
                <div className="message-body">
                    Sorry, the contact form is currently unavailable. Please try again later.
                </div>
            </article>
            
            <article className={ submitted ? 'message is-primary is-visible' : 'is-hidden'  }>
                <div className="message-header">
                    <p></p>
                    <button className="delete" aria-label="delete"
                    onClick={() => setSubmitted(false) }></button>
                </div>
                <div className="message-body">
                    Thanks, I'll be in contact shortly
                </div>
            </article>
        </>
    )
}